import m from 'mithril';

const initLogin = payload => {
  Model.login();
  return payload;
};

const getProfile = payload => {
  const pid = payload.data_profiles[0].profile_id;
  Promise.all([Model.profile(pid), Model.fields(pid)]).then(data => {
    let p = Model.payload;
    p.profile = data[0];
    p.fields = data[1];
  });
  return payload;
};

const getDetails = payload => {
  let p = Model.payload;
  payload.data_profiles.forEach(e => {
    if (e.profile_id === parseInt(process.env.EDMS_PROFILE_ID)) {
      p.userid = e.data_columns.find(
        f => f.col_desc.includes(process.env.EDMS_UID_COL_DESC) === true
      ).profile_value;
      if (e.length !== '') {
        p.details.push({
          image: e.image_name,
          type: e.data_columns.find(
            f => f.col_desc.includes(process.env.EDMS_DTYPE_COL_DESC) === true
          ).profile_value,
          ver_id: e.ver_id,
          doc_id: e.doc_id
        });
      }
    }
  });
  //console.log(Model.payload);
  return Model.payload;
};

const Model = {
  error: '',
  payload: {
    userid: '',
    profile: '',
    fields: [],
    details: []
  },

  profile: id => {
    return m
      .request({
        method: 'GET',
        url: '/api/v1/students/get-profile-by-id/' + id,
        withCredentials: true
      })
      .then(result => {
        return result.profiles[0];
      });
  },

  fields: id => {
    return m
      .request({
        method: 'GET',
        url: '/api/v1/students/get-profile-fields/' + id,
        withCredentials: true
      })
      .then(result => {
        return result.profile_fields;
      });
  },

  search: uid => {
    return m
      .request({
        method: 'GET',
        url: '/api/v1/students/search',
        data: { criteria: uid },
        withCredentials: true
      })
      .then(initLogin)
      .then(getProfile)
      .then(getDetails)
      .catch(error => {
        Model.error = error.message;
      });
    //use json file for offline/test
    //initFetchJSON(uid);
  },

  login: () => {
    return m
      .request({
        method: 'GET',
        url: '/api/v1/students/login/',
        data: {
          username: process.env.EDMS_USERNAME,
          userpassword: process.env.EDMS_PASSWORD
        },
        withCredentials: true
      })
      .then(result => {
        return console.log(result.status);
      })
      .catch(error => {
        Model.error = error.message;
      });
  },

  download: (event, payload) => {
    event.preventDefault();
    return m
      .request({
        method: 'GET',
        url: '/api/v1/students/download',
        data: {
          ver_id: parseInt(payload.ver_id),
          profile_id: parseInt(process.env.EDMS_PROFILE_ID),
          file_type: 1
        },
        withCredentials: true
      })
      .then(({ download }) => {
        if (download.download_result !== '1') return alert('Fail to download!');
        //conver base64 to blob
        const blob = b64toBlob(download.file_content, 'image/png');
        const blobUrl = URL.createObjectURL(blob);
        //create a href download element
        const link = document.createElement('a');
        link.href = blobUrl;
        link.setAttribute('download', `${download.filename}`);
        // append to html page
        document.body.appendChild(link);
        // force download
        link.click();
        // clean up node
        link.parentNode.removeChild(link);
      })
      .catch(error => {
        Model.error = error.message;
      });
  }
};

const b64toBlob = (b64Data, contentType = '', sliceSize = 512) => {
  const byteCharacters = atob(b64Data);
  const byteArrays = [];
  for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
    const slice = byteCharacters.slice(offset, offset + sliceSize);
    const byteNumbers = new Array(slice.length);
    for (let i = 0; i < slice.length; i++) {
      byteNumbers[i] = slice.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    byteArrays.push(byteArray);
  }
  const blob = new Blob(byteArrays, { type: contentType });
  return blob;
};

// test methods start
const initFetchJSON = uid => {
  try {
    if (uid === 'msu001') {
      fetchJSONFile('payload.json', function(data) {
        Promise.resolve(data)
          .then(getProfile)
          .then(getDetails);
      });
    } else {
      throw 'unknown user';
    }
  } catch (error) {
    Model.error = error;
  }
};

function fetchJSONFile(path, callback) {
  var httpRequest = new XMLHttpRequest();
  httpRequest.onreadystatechange = function() {
    if (httpRequest.readyState === 4) {
      if (httpRequest.status === 200) {
        var data = JSON.parse(httpRequest.responseText);
        if (callback) callback(data);
      }
    }
  };
  httpRequest.open('GET', path);
  httpRequest.send();
}
// test methods end

export default Model;

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
  }
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

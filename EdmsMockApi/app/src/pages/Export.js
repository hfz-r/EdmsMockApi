import m from 'mithril';
import DropZone from '../components/DropZone';
import ProgressBar from '../components/ProgressBar';
import Model from '../model';

export const Export = () => {
  let prevFiles = [];
  let state = {
    files: [],
    uploading: false,
    uploadProgress: {},
    successfullUploaded: false
  };

  const onFilesAdded = filesAdded => {
    const result = filesAdded.filter(
      o1 => !prevFiles.some(o2 => o1.name === o2.name)
    );
    state.files = prevFiles.concat(result);
  };

  const uploadFiles = async () => {
    state.uploadProgress = {};
    state.uploading = true;
    const promises = [];
    state.files.forEach(file => {
      promises.push(sendRequest(file));
    });
    try {
      await Promise.all(promises);

      state.successfullUploaded = true;
      state.uploading = false;
    } catch (e) {
      state.successfullUploaded = true;
      state.uploading = false;
    }
  };

  const sendRequest = file => {
    return new Promise((resolve, reject) => {
      const req = new XMLHttpRequest();

      req.upload.addEventListener('progress', event => {
        if (event.lengthComputable) {
          const copy = { ...state.uploadProgress };
          copy[file.name] = {
            state: 'pending',
            percentage: (event.loaded / event.total) * 100
          };
          state.uploadProgress = copy;
          m.redraw();
        }
      });

      req.upload.addEventListener('load', event => {
        const copy = { ...state.uploadProgress };
        copy[file.name] = { state: 'done', percentage: 100 };
        state.uploadProgress = copy;
        m.redraw();
        resolve(req.response);
      });

      req.upload.addEventListener('error', event => {
        const copy = { ...state.uploadProgress };
        copy[file.name] = { state: 'error', percentage: 0 };
        state.uploadProgress = copy;
        reject(req.response);
      });

      req.open('POST', '/api/v1/students/export');
      req.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');

      const reader = new FileReader();
      reader.onload = function(event) {
        const base64 = event.target.result.split('base64,')[1],
          params = {
            file_content: base64,
            file_name: file.name,
            profile_id: process.env.EDMS_PROFILE_ID,
            profile_value: [Model.payload.userid, 'Profile Pictures'],
            folder_name: process.env.EDMS_EXPORT_FOLDER,
            user_name: process.env.EDMS_USERNAME
          };
        req.send(JSON.stringify(params));
      };
      reader.readAsDataURL(file);
    });
  };

  const renderProgress = file => {
    const uploadProgress = state.uploadProgress[file.name];
    if (state.uploading || state.successfullUploaded) {
      return (
        <div className="ProgressWrapper">
          <ProgressBar
            progress={uploadProgress ? uploadProgress.percentage : 0}
          />
          <div className="ImageWrapper">
            <img
              className="CheckIcon"
              alt="done"
              src="dist/baseline-check_circle_outline-24px.svg"
              style={{
                opacity:
                  uploadProgress && uploadProgress.state === 'done' ? 0.5 : 0
              }}
            />
          </div>
        </div>
      );
    }
  };

  const renderActions = () => {
    if (state.successfullUploaded) {
      return (
        <button
          onclick={() => {
            state.files = [];
            state.successfullUploaded = false;
          }}
        >
          Clear
        </button>
      );
    } else {
      return (
        <button
          disabled={state.files.length < 0 || state.uploading}
          onclick={() => uploadFiles()}
        >
          Upload
        </button>
      );
    }
  };

  const removeFile = file => {
    const index = state.files.indexOf(file);
    if (index > -1) {
      state.files.splice(index, 1);
    }
  };

  return {
    oninit: vnode => {
      vnode.state.state = state;
    },
    onbeforeupdate: (vnode, old) => {
      return (prevFiles = old.state.state.files);
    },
    view: vnode => {
      return (
        <div>
          <div className="Content">
            <DropZone
              filesAdded={onFilesAdded}
              disabled={state.uploading || state.successfullUploaded}
            />
            <div className="Files">
              {state.files.map(file => {
                return (
                  <div key={file.name} className="Row">
                    <div>
                      <span className="Filename">{file.name}</span>
                      <i
                        class="fa fa-window-close pull-right mr-1"
                        onclick={
                          state.uploading || state.successfullUploaded
                            ? () => alert('State disabled!')
                            : () => removeFile(file)
                        }
                      ></i>
                    </div>
                    {renderProgress(file)}
                  </div>
                );
              })}
            </div>
          </div>
          <div className="Actions">{renderActions()}</div>
        </div>
      );
    }
  };
};

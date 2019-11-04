import m from 'mithril';

const DropZone = () => {
  let _domFileInput = '';
  let hightlight = false;
  let disabled = false;
  let filesAdded = [];

  const openFileDialog = () => {
    if (disabled) return;
    _domFileInput.click();
  };

  const onFilesAdded = event => {
    if (disabled) return;
    const files = event.target.files;
    if (filesAdded) {
      const array = fileListToArray(files);
      filesAdded(array);
    }
  };

  const onDragOver = event => {
    event.preventDefault();
    if (disabled) return;
    hightlight = true;
    m.redraw();
  };

  const onDragLeave = () => {
    hightlight = false;
    m.redraw();
  };

  const onDrop = event => {
    event.preventDefault();
    if (disabled) return;
    const files = event.dataTransfer.files;
    if (filesAdded) {
      const array = fileListToArray(files);
      filesAdded(array);
    }
    hightlight = false;
    m.redraw();
  };

  const fileListToArray = list => {
    const array = [];
    for (var i = 0; i < list.length; i++) {
      array.push(list.item(i));
    }
    return array;
  };

  return {
    oncreate: vnode => {
      //dnd micro-library
      vnode.dom.addEventListener('dragover', onDragOver);
      vnode.dom.addEventListener('dragleave', onDragLeave);
      vnode.dom.addEventListener('drop', onDrop);
    },
    view: vnode => {
      disabled = vnode.attrs.disabled;
      filesAdded = vnode.attrs.filesAdded;

      return (
        <div
          className={`Dropzone ${hightlight ? 'Highlight' : ''}`}
          dragover={onDragOver}
          dragleave={onDragLeave}
          drop={onDrop}
          onclick={openFileDialog}
          style={{ cursor: disabled ? 'default' : 'pointer' }}
        >
          <input
            className="FileInput"
            type="file"
            multiple
            oncreate={vnode => (_domFileInput = vnode.dom)}
            onchange={onFilesAdded}
          />
          <img
            alt="upload"
            className="Icon"
            src="dist/baseline-cloud_upload-24px.svg"
          />
          <span>Drag and drop a file here or click</span>
        </div>
      );
    }
  };
};

export default DropZone;

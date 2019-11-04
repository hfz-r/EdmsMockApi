import m from "mithril";

const ProgressBar = () => {
  return {
    view: vnode => {
      return (
        <div className="ProgressBar">
          <div
            className="Progress"
            style={{ width: vnode.attrs.progress + "%" }}
          />
        </div>
      );
    }
  };
};

export default ProgressBar;

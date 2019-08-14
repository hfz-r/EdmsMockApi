import m from 'mithril';

export const Result = () => {
  return {
    view: vnode => {
      return (
        <div className="tab-pane" id={vnode.attrs.id}>
          <div className="row">
            <h4 className="info-text"> {vnode.attrs.info}</h4>

            <div className="col-sm-6 col-sm-offset-1">
              <div className="form-group">
                <label>Room description</label>
                <textarea className="form-control" placeholder="" rows="6" />
              </div>
            </div>
            <div className="col-sm-4">
              <div className="form-group">
                <label className="control-label">Example</label>
                <p className="description">
                  "The room really nice name is recognized as being a really
                  awesome room. We use it every sunday when we go fishing and we
                  catch a lot. It has some kind of magic shield around it."
                </p>
              </div>
            </div>
          
          </div>
        </div>
      );
    }
  };
};

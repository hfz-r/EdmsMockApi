import m from 'mithril';
import { Icon } from '../components/Icon';
import { Select } from '../components/Select';
import Model from '../models';

export const Begin = () => {
  const fields = () => {
    const items = [];

    items.push(
      <div className="input-group">
        <span className="input-group-addon">
          <Icon iconName="face" />
        </span>
        <div className="form-group label-floating">
          <label className="control-label">User-Id</label>
          <input
            name="userid"
            type="text"
            className="form-control"
            value={Model.userid}
            oninput={e => Model.setUserId(e.target.value)}
          />
        </div>
      </div>
    );

    items.push(
      <div className="input-group">
        <span className="input-group-addon">
          <Icon iconName="assignment" />
        </span>
        <Select
          label="Profile"
          options={Model.profiles.map(i => {
            const c = {};
            c.value = i.profile_id;
            c.label = i.profile_name;
            return c;
          })}
          onchange={v => {}}
        />
      </div>
    );
    return items;
  };

  return {
    oninit: Model.loadProfiles,
    view: vnode => {
      return (
        <div class="tab-pane" id={vnode.attrs.id}>
          <div class="row">
            <h4 class="info-text">{vnode.attrs.info}</h4>
            <div className="col-sm-10 col-sm-offset-1">{fields()}</div>
          </div>
        </div>
      );
    }
  };
};

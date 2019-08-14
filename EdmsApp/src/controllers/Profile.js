import m from 'mithril';
import { Icon } from '../components/Icon';
import { Select } from '../components/Select';
import Model from '../models';

export const Profile = () => {
  const fields_ = () => {
    const items = [];

    items.push(
      <div className="input-group">
        <span className="input-group-addon">
          <Icon iconName="face" />
        </span>
        <div className="form-group label-floating">
          <label className="control-label">Username</label>
          <input
            name="username"
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
          onchange={v => {
            if (v !== '') {
              Model.loadFields(v);
            }
          }}
        />
      </div>
    );
    return items;
  };
  const fields__ = () => {
    const items = [];

    items.push(
      <div
        className="choice"
        data-toggle="wizard-checkbox"
        rel="tooltip"
        title="Check this to activate logical operator 'OR' in your search conditions."
      >
        <input type="checkbox" name="operator" />
        <div
          className="icon"
          onclick={() =>
            Model.setOperator($('input[name="operator"]').prop('checked'))
          }
        >
          <i className="fa fa-plus" />
        </div>
        <h6>Operator</h6>
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
            <div className="col-sm-6 col-sm-offset-1">{fields_()}</div>
            <div className="col-sm-5">{fields__()}</div>
          </div>
        </div>
      );
    }
  };
};

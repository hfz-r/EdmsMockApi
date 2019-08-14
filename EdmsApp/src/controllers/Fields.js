import m from 'mithril';
import { Icon } from '../components/Icon';
import Model from '../models';

export const Fields = () => {
  let cnt = 0;
  const obj = {
    number: {
      type: 'number',
      icon: <Icon iconName="exposure_plus_2" />
    },
    date: {
      type: 'date',
      icon: <Icon iconName="date_range" />
    },
    text: {
      type: 'text',
      icon: <Icon iconName="text_fields" />
    }
  };

  const grid = y => {
    const records = [];
    for (let i = 0; i < y; i++) {
      const f = Model.fields[cnt++];
      const p = datatype(f.col_datatype);
      records.push(
        <div key={f.col_id} className="input-group">
          <span className="input-group-addon">{p.icon}</span>
          <div className="form-group label-floating">
            <label className="control-label">{f.col_desc}</label>
            <input
              key={f.col_id}
              name={f.col_name}
              type={p.type}
              className="form-control"
              value=""
            />
          </div>
        </div>
      );
    }
    return records;
  };

  const datatype = d => {
    switch (d) {
      case 'number':
      case 'numeric':
        return obj.number;
      case 'datetime':
      case 'date':
        return obj.date;
      default:
        return obj.text;
    }
  };

  const fields = () => {
    const row = Math.floor(Model.fields.length / 2);
    const remainder = Model.fields.length % 2;
    const items = [];
    for (let d = 0; d < 2; d++) {
      let y = row;
      d === 0 && remainder === 1 ? (y = y + 1) : (y = row);
      items.push(
        <div key={d + 1} className="col-sm-6">
          {grid(y)}
        </div>
      );
    }
    cnt = 0;
    return items;
  };

  const fields_ = () => {
    return (
      <div className="col-sm-10 col-sm-offset-1">
        <div className="input-group" />
      </div>
    );
  };

  return {
    onupdate: vnode => Model.initValidation(Model.operator),
    view: vnode => {
      return (
        <div className="tab-pane" id={vnode.attrs.id}>
          <div className="row">
            <h4 className="info-text">{vnode.attrs.info}</h4>
            {Model.fields && Model.fields.length !== 0
              ? Model.fields.length == 1
                ? fields_()
                : fields()
              : ''}
          </div>
        </div>
      );
    }
  };
};

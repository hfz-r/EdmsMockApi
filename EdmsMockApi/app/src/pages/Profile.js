import m from 'mithril';
import { groupByArray } from '../common/helpers';
import { Body } from '../components/Body';
import Model from '../model';

export const Profile = () => {
  let payload = Model.payload.details;

  let options = ['All'];
  let grouped = groupByArray(payload, 'type');
  grouped
    .map(o => [o.key])
    .flat()
    .forEach(e => {
      options.push(e);
    });

  return {
    view: vnode => {
      return (
        <div>
          <div class="d-flex justify-content-end pb-3">
            <div class="form-inline">
              <label class="text-muted mr-3" for="order-sort">
                Sort Images
              </label>
              <select
                class="form-control"
                id="order-sort"
                onchange={e =>
                  (payload =
                    e.target.value === 'All'
                      ? Model.payload.details
                      : Model.payload.details.filter(
                          m => m.type === e.target.value
                        ))
                }
              >
                {options.map(t => (
                  <option>{t}</option>
                ))}
              </select>
            </div>
          </div>
          <div class="table-responsive">
            <table class="table table-hover mb-0">
              <thead>
                <tr>
                  <th>Image #</th>
                  <th>Version</th>
                  <th>Type</th>
                  <th>doc_id</th>
                </tr>
              </thead>
              <tbody>
                <Body payload={payload} groupby={grouped} />
              </tbody>
            </table>
          </div>
        </div>
      );
    }
  };
};

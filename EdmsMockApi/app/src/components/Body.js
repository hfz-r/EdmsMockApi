import m from 'mithril';
import Model from '../model';

export const Body = () => {
  return {
    view: ({ attrs: { payload, groupby } }) => {
      return payload.map(p => {
        return (
          <tr>
            <td style="width:55%">
              <a
                class="navi-link"
                href={`${process.env.EDMS_IMG_PATH}${p.image}`}
                data-toggle="lightbox"
                data-gallery="example-gallery"
              >
                {p.image}
              </a>
            </td>
            {/* <td style="width:10%">{`v${p.ver_id}`}</td> */}
            <td style="width:25%">
              {groupby
                .filter(g => g.key === p.type)
                .map(s => {
                  return <span class={`badge ${s.badge} m-0`}>{s.key}</span>;
                })}
            </td>
            <td style="width:20%">{p.doc_id}</td>
            <td style="width:10%">
              <a href="#" onclick={event => Model.download(event, p)}>
                <i class="fa fa-download"></i>
              </a>
            </td>
          </tr>
        );
      });
    }
  };
};

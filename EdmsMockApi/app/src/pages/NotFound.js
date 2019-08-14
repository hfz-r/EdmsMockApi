import m from 'mithril';
import { Loader } from '../components/Loader';
import { EmptyState } from 'construct-ui';

export const NotFound = () => {
  let spinner = true;

  return {
    oninit: vnode => {
      setTimeout(() => {
        spinner = false;
        m.redraw();
      }, 2000);
    },
    view: ({ attrs: { icon, header, content = '' } }) => {
      return (
        <div class="row">
          {spinner ? (
            <Loader active={true} />
          ) : (
            <EmptyState icon={icon} header={header} content={content} />
          )}
        </div>
      );
    }
  };
};

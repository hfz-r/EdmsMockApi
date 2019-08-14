import m from 'mithril';
import Model from '../model';
import { Icons } from 'construct-ui';
import { Loader } from '../components/Loader';
import { Content } from './Content';
import { NotFound } from './NotFound';

export const Stage = () => {
  let spinner = true;
  return {
    oninit: vnode => {
      Model.search(vnode.attrs.uid);
    },
    view: vnode => {
      return (
        <div class="container mb-4 main-container">
          {Model.error && Model.error.length !== 0 ? (
            <NotFound
              icon={Icons.USER_X}
              header="No search results found."
              content={
                <div>
                  <pre class="pre-scrollable" style="text-align:left">
                    {Model.error}
                  </pre>
                </div>
              }
            />
          ) : Model.payload && Model.payload.userid.length !== 0 ? (
            <Content page={vnode.attrs.page} uid={vnode.attrs.uid} />
          ) : (
            <Loader active={spinner} />
          )}
        </div>
      );
    }
  };
};

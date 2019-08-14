import m from 'mithril';
import Main from 'layouts/Main';

export { generate } from 'routes/generate';

export var routes = {
  profile: {
    path: '/profile/:uid',
    render: vnode => {
      return <Main uid={vnode.attrs.uid} />;
    }
  }
};

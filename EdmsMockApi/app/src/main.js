import m from 'mithril';
import { Icons } from 'construct-ui';
import { NotFound } from './pages/NotFound';
import { Stage } from './pages/Stage';

import './styles/main.scss';

m.route.prefix('');
m.route(document.getElementById('app'), '/', {
  '/': {
    render: () => {
      return <NotFound icon={Icons.ALERT_TRIANGLE} header="Unknown request." />;
    }
  },
  '/:page/:uid': {
    render: vnode => {
      return <Stage page={vnode.attrs.page} uid={vnode.attrs.uid} />;
    }
  }
});

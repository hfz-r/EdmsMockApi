import m from 'mithril';
import { Spinner, Overlay } from 'construct-ui';

export const Loader = () => {
  let active = false;

  const style = {
    position: 'absolute',
    top: '30px',
    right: '30px',
    bottom: 0,
    zIndex: 100
  };

  const content = (
    <div style={style}>
      <Spinner active={!active} size="xl" intent="negative" />
    </div>
  );

  return {
    oninit: vnode => {
      active = vnode.attrs.active;
    },
    view: vnode => {
      return (
        <Overlay
          closeOnEscapeKey={false}
          closeOnOutsideClick={false}
          content={content}
          isOpen={active}
          onClose={() => (active = false)}
        />
      );
    }
  };
};

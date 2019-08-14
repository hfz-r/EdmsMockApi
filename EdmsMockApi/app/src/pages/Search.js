import m from 'mithril';
import { Dialog, Input, Button, Classes } from 'construct-ui';

export const Search = () => {
  let isOpen = false;
  let isSubmitting = false;
  let payload = '';

  const Close = () => {
    isOpen = false;
    isSubmitting = false;
  };

  const Submit = () => {
    isSubmitting = true;
    setTimeout(() => {
      Close();
      m.redraw();
      window.location.href = `/1/${payload}`;
    }, 1000);
  };

  const setInput = txt => (payload = txt);

  return {
    oninit: vnode => (isOpen = vnode.attrs.open),
    view: vnode => {
      return (
        <div>
          <Dialog
            closeOnOutsideClick={false}
            closeOnEscapeKey={false}
            content={
              <Input
                autofocus={true}
                fluid={true}
                oninput={e => setInput(e.target.value)}
              />
            }
            hasBackdrop={true}
            isOpen={isOpen}
            inline={false}
            onClose={Close}
            title="Please search?"
            footer={
              <div class={Classes.ALIGN_RIGHT}>
                <Button label="Close" onclick={Close} />
                <Button
                  label="Submit"
                  loading={isSubmitting}
                  intent="primary"
                  onclick={Submit}
                />
              </div>
            }
          />
        </div>
      );
    }
  };
};

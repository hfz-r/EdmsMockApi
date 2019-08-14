import m from 'mithril';

export const Header = () => {
  return {
    view: vnode => {
      return (
        <div className="wizard-header">
          <h3 className="wizard-title">Mock Up App</h3>
          <h5>This mock-up will excersice the usage of the EDMS API.</h5>
        </div>
      );
    }
  };
};

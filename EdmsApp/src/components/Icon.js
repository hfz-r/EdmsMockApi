import m from 'mithril';

export const Icon = () => {
  return {
    view: ({ attrs: { iconName } }) => {
      return <i className="material-icons">{iconName}</i>;
    }
  };
};

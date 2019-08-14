import m from 'mithril';

export const Checkbox = () => {
  return {
    view: ({ attrs }) => {
      const { name, onchange, description, checked, disabled } = attrs;
      return (
        <div class="checkbox">
          <label>
            <input
              type="checkbox"
              name={name}
              checked={checked}
              disabled={disabled}
              onchange={
                onchange
                  ? e => {
                      if (e.target && typeof e.target.checked !== 'undefined') {
                        onchange(e.target.checked);
                      }
                    }
                  : undefined
              }
            />
          </label>
          {description}
        </div>
      );
    }
  };
};

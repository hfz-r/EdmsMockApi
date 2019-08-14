import m from 'mithril';

export const Select = () => {
  const renderOption = option => {
    const label = typeof option === 'object' ? option.label : option;
    const value = typeof option === 'object' ? option.value : option;
    return <option value={value}>{label}</option>;
  };

  return {
    view: ({ attrs }) => {
      const {
        className = 'form-group label-floating',
        options = [],
        label,
        disabled,
        onchange
      } = attrs;

      const isDisabled = disabled ? '[disabled]' : '';
      const selectOptions = options.map(option => renderOption(option));

      return (
        <div className={className}>
          <label className="control-label">{label}</label>
          <select
            name="select"
            className="form-control"
            disabled={isDisabled}
            onchange={
              onchange
                ? e => {
                    if (e && e.currentTarget) {
                      const b = e.currentTarget;
                      onchange(b.value);
                    }
                  }
                : undefined
            }
          >
            <option disabled="" selected="" />
            {selectOptions}
          </select>
        </div>
      );
    }
  };
};

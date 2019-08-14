import m from 'mithril';

export const Navigation = () => {
  const style1 = (txt1, icon, route, active = '') => {
    return (
      <a
        class={`list-group-item ${active}`}
        href={route}
        oncreate={m.route.Link}
      >
        <i class={`${icon} text-muted`} />
        {txt1}
      </a>
    );
  };

  const style2 = (txt1, txt2, icon, route, active = '') => {
    return (
      <a
        class={`list-group-item ${active}`}
        href={route}
        oncreate={m.route.Link}
      >
        <div class="d-flex justify-content-between align-items-center">
          <div>
            <i class={`${icon}  mr-1 text-muted`} />
            <div class="d-inline-block font-weight-medium text-uppercase">
              {txt1}
            </div>
          </div>
          <span class="badge badge-secondary">{txt2}</span>
        </div>
      </a>
    );
  };

  return {
    view: ({ attrs: { uid, contents, position } }) => {
      return (
        <div class="wizard">
          <nav class="list-group list-group-flush">
            {contents.map((c, i) => {
              return c.style === 'style1'
                ? style1(
                    c.title,
                    c.icon,
                    `/${i + 1}/${uid}`,
                    position === i ? 'active' : ''
                  )
                : style2(
                    c.title,
                    c.txt2 ? c.txt2 : '',
                    c.icon,
                    `/${i + 1}/${uid}`,
                    position === i ? 'active' : ''
                  );
            })}
          </nav>
        </div>
      );
    }
  };
};

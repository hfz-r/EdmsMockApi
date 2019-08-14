import m from 'mithril';

export const Content = () => {
  return {
    view: ({ attrs: { contents } }) => {
      return (
        <content>
          <div class="wizard-navigation">
            <ul>
              {contents.map(content => (
                <li key={content.id}>
                  <a href={content.id} data-toggle="tab">
                    {content.description}
                  </a>
                </li>
              ))}
            </ul>
          </div>
          <div class="tab-content">
            {contents.map(content => content.content)}
          </div>
        </content>
      );
    }
  };
};

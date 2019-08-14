import m from 'mithril';
import Model from '../model';
import { Navigation } from '../components/Navigation';
import { Profile } from './Profile';
import { Search } from './Search';

export const Content = () => {
  const contents = [
    {
      title: 'Profile',
      icon: 'fa fa-list-alt',
      style: 'style2',
      txt2: Model.payload.details.length,
      body: () => <Profile />
    },
    {
      title: 'Advanced Search',
      icon: 'fa fa-search-plus',
      style: 'style1',
      body: () => <Search open={true} />
    }
  ];

  const clamp = (n, min, max) => {
    return Math.min(Math.max(n, min), max);
  };

  return {
    view: ({ attrs: { page, uid } }) => {
      const position = clamp((Number(page) || 1) - 1, 0, contents.length - 1);
      return (
        <div class="row">
          <div class="col-lg-4 pb-5">
            <div class="author-card pb-3">
              <div
                class="author-card-cover"
                style="background-image: url(dist/Netflix.jpg);"
              />
              <div class="author-card-profile">
                <div class="author-card-avatar">
                  <img
                    src="https://via.placeholder.com/300"
                    alt={Model.payload.userid}
                  />
                </div>
                <div class="author-card-details">
                  <h5 class="author-card-name text-lg">
                    {Model.payload.userid}
                  </h5>
                  <span class="author-card-position">
                    {Model.payload.profile.profile_name}
                  </span>
                </div>
              </div>
            </div>
            <Navigation uid={uid} contents={contents} position={position} />
          </div>
          <div class="col-lg-8 pb-5">
            {contents.map((c, i) => {
              return i === position ? c.body() : '';
            })}
          </div>
        </div>
      );
    }
  };
};

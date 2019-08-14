import m from 'mithril';
import Wizard from '../components/Wizard/index';
import Model from '../models/index';

export default class Main {
  constructor(vnode) {
    Model.userid = vnode.attrs.uid;
  }

  view() {
    return (
      <div
        class="image-container set-full-height"
        style="background-image: url('img/Netflix.jpg')"
      >
        <div class="container">
          <div class="row">
            <div class="col-sm-8 col-sm-offset-2">
              <Wizard />
            </div>
          </div>
        </div>
        <div class="footer">
          <div class="container text-center">
            Made with <i class="fa fa-heart heart" /> by <a href="#">hfz-r</a>.
          </div>
        </div>
      </div>
    );
  }
}

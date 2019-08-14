import m from 'mithril';
import { Checkbox } from '../Checkbox';
import Model from '../../models';

export const Footer = () => {
  return {
    view: vnode => {
      return (
        <div class="wizard-footer">
          <div class="pull-right">
            <input
              type="button"
              class="btn btn-next btn-fill btn-danger btn-wd"
              name="next"
              value="Next"
            />
            <input
              type="button"
              class="btn btn-finish btn-fill btn-danger btn-wd"
              name="finish"
              value="Finish"
            />
          </div>
          <div class="pull-left">
            <input
              type="button"
              class="btn btn-previous btn-fill btn-default btn-wd"
              name="previous"
              value="Previous"
            />

            <div class="footer-checkbox">
              <div class="col-sm-12">
                <Checkbox
                  name="optionsCheckboxes"
                  description="I have read and agree to the terms of service"
                  onchange={checkedId =>
                    alert(`Options ${checkedId} are checked.`)
                  }
                />
              </div>
            </div>
          </div>
          <div class="clearfix" />
        </div>
      );
    }
  };
};

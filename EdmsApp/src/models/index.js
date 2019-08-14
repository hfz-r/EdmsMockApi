import m from 'mithril';

const Model = {
  userid: '',
  setUserId: value => {
    Model.userid = value;
  },

  operator: true,
  setOperator: value => {
    Model.operator = value;
  },

  profiles: [],
  loadProfiles: () => {
    return m
      .request({
        method: 'GET',
        url: 'http://localhost:5000/api/v1/students/get-profiles'
        // withCredentials: true
      })
      .then(result => {
        Model.profiles = result.profiles;
      });
  },

  fields: [],
  loadFields: id => {
    return m
      .request({
        method: 'GET',
        url: 'http://localhost:5000/api/v1/students/get-profile-fields/:id',
        data: { id: id }
        //withCredentials: true
      })
      .then(result => {
        Model.fields = result.profile_fields;
      });
  },

  initValidation: operator => {
    Model.fields.map(f => {
      const m = $(`input[name="${f.col_name}"]`);
      if (m.length > 0) {
        Model.resetValidation(m);
        if (operator) {
          m.rules('add', {
            required: true
          });
        } else {
          $.validator.addMethod('optional', function(value, element) {
            var regExp = new RegExp(/^[a-zA-Z0-9\s|\,|\.|\-|\']+$/);
            return this.optional(element) || regExp.test(value);
          });
          m.rules('add', {
            optional: true
          });
        }
      }
    });
  },

  resetValidation: m => {
    m.rules('remove');
    m.parent('div').removeClass('has-error');
  }
};

export default Model;

import Component from '@ember/component';
import hbs from 'htmlbars-inline-precompile';
import { connect } from 'ember-redux';
import { tagName } from '@ember-decorators/component';

const stateToComputed = state => {
  const {
    isLoading
  } = state.global;

  return {
    isLoading
  };
};

const dispatchToActions = {};

@tagName('')
class ApplicationContainer extends Component {
  layout = hbs`{{yield (hash
    isLoading=isLoading
  )}}`;
}

export default connect(stateToComputed, dispatchToActions)(ApplicationContainer);
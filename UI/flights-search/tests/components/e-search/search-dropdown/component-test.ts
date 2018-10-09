import { module, test } from 'qunit';
import { setupRenderingTest } from 'ember-qunit';
import hbs from 'htmlbars-inline-precompile';

import { find } from '@ember/test-helpers';
import a11yAudit from 'ember-a11y-testing/test-support/audit';

module('e-search/search-dropdown', function(hooks) {
  setupRenderingTest(hooks);

  test('it renders', async function(assert) {
    await this.render(hbs`{{e-search/search-dropdown}}`);

    assert.dom('[data-test-e-search/search-dropdown]').exists();

    await a11yAudit({});
    assert.ok(true, 'no a11y errors found!');
  });
});
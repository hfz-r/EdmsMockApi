import m from 'mithril';
import { Content } from './Content';
import { Header } from './Header';
import { Footer } from './Footer';
import { Result } from '../../controllers/Result';
import { Begin } from '../../controllers/Begin';

export default class Wizard {
  content() {
    return [
      {
        id: '#begin',
        description: 'Verify Search Data',
        content: <Begin id="begin" info="Begin search." />
      },
      {
        id: '#result',
        description: 'Output',
        content: (
          <Result id="result" info="Done!" />
        )
      }
    ];
  }

  view() {
    return (
      <div class="wizard-container">
        <div class="card wizard-card" data-color="red" id="wizard">
          <form action="" method="">
            <Header />
            <Content contents={this.content()} />
            <Footer />
          </form>
        </div>
      </div>
    );
  }
}

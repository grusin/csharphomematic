import React from 'react';
import * as Ons from 'react-onsenui';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import 'whatwg-fetch'

class BaseComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = { 
      ui: { 
        toolbar: { 
          name: "",
          showBackButton: false 
        }        
      } 
    };
  }

  componentDidMount() {
    this.componentNeedsData(null);
  }

  gotoComponent(component, key) {
    this.props.navigator.pushPage({ comp: component, props: key });
  }

  pullHookChange(event) {
    this.setState({
      pullHookState: event.state
    });
  }

  pullHookLoad(done) {
    this.componentNeedsData(done);
  }

  componentNeedsData(done) {
  }

  renderPullHook() {
    switch (this.state.pullHookState) {
      case 'action':
        return <div>Loading...<br /><Ons.Icon icon='spinner' spin /></div>;
      default:
        return;
    }
  }

  renderToolbar() {
    return (<Ons.Toolbar>
      {this.state.ui.toolbar.showBackButton === true &&
        <div className='left'>
          <Ons.BackButton>Back</Ons.BackButton>
        </div>
      }
      <div className='center'>{this.state.ui.toolbar.name}</div>
    </Ons.Toolbar>
    );
  }

  renderPage() {
    return
  }

  render() {
    return (
      <Ons.Page renderToolbar={this.renderToolbar.bind(this)}>
        <Ons.PullHook onChange={this.pullHookChange.bind(this)} onLoad={this.pullHookLoad.bind(this)}>
          {this.renderPullHook()}
        </Ons.PullHook>
        {this.renderPage()}
      </Ons.Page>
    );
  }
}

export default BaseComponent;
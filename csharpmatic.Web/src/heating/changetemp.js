import React from 'react';
import * as Ons from 'react-onsenui';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import BaseComponent from '../BaseComponent.js';
import App from '../index.js';

class ChangeTemp extends BaseComponent {
  constructor(props) {
    super(props);

    this.state.room = {};
    this.state.ui.toolbar.name = "Change Temperature";
    this.state.ui.toolbar.showBackButton = true;
    this.state.ui.disabled = true;
    this.state.DesiredTemp = null;
    this.state.DesiredTempIdx = null;
  }


  idx2Temp(idx) {
    //assume that min temp is 5, max temp is 30, round to 0.5 deg
    return Math.round((5 + idx / 4.0) * 2.0) / 2.0;
  }

  temp2idx(temp) {
    return (temp - 5) * 4;
  }

  componentNeedsData(done) {
    App.Api_GetRoom(this.props.ISEID).then(data => {
      this.setState({ room: data });

      //initial data load, set seme extra magic
      if (this.state.DesiredTemp == null) {
        this.setState({
          DesiredTemp: this.state.room.SetTemp,
          DesiredTempIdx: this.temp2idx(this.state.room.SetTemp)
        });

        this.setState(state => { state.ui.disabled = false; return state });
      }

      if (done != null)
        done();
    }
    );
  }

  handleBoostSwitch(event) {
    //console.log(event);

    //disable UI
    this.setState(state => { state.ui.disabled = true; return state });

    App.Api_SetRoomBoost(this.props.ISEID, event.value).then(data => {
      this.setState({ room: data });
      this.setState(state => { state.ui.disabled = false; return state });
    });
  }

  handleApplyChanges() {
    console.log(this.state);

    //disable UI
    this.setState(state => { state.ui.disabled = true; return state });

    //set new state
    App.Api_SetRoomTemp(this.props.ISEID, this.state.DesiredTemp).then(data => {
      this.setState({ room: data });
      this.setState(state => { state.ui.disabled = false; return state });
    });
  }

  renderPage() {
    return (
      <Ons.List>
        <Ons.ProgressBar indeterminate={this.state.ui.disabled} />
        <Ons.ListHeader>{this.state.room.Name}</Ons.ListHeader>
        <Ons.ListItem>
          <div className='left'>Set Temp:</div>
          <div className='right'>{this.state.room.SetTemp > 0 && this.state.room.SetTemp.toFixed(1)}&#8451;</div>
        </Ons.ListItem>
        <Ons.ListItem>
          <div className='left'>Actual Temp:</div>
          <div className='right'>{this.state.room.ActualTempAvg > 0 && this.state.room.ActualTempAvg.toFixed(1)}&#8451;</div>
        </Ons.ListItem>
        <Ons.ListHeader>Heat boost</Ons.ListHeader>
        <Ons.ListItem>
          <div className='left'>Boost <Ons.Icon icon="fa-rocket" /> <Ons.Icon icon="fa-rocket" /> <Ons.Icon icon="fa-rocket" />:</div>
          <div className='right'><Ons.Switch checked={this.state.room.BoostActive} onChange={this.handleBoostSwitch.bind(this)} disabled={this.state.ui.disabled} /></div>
        </Ons.ListItem>
        <Ons.ListHeader>Your desired Temp</Ons.ListHeader>
        <Ons.ListItem>
          <div className='left'>Desired Temp:</div>
          <div className='right'>{this.state.DesiredTemp > 0 && this.state.DesiredTemp.toFixed(1)}&#8451;</div>
        </Ons.ListItem>
        <Ons.ListItem>
          <Ons.Range value={this.state.DesiredTempIdx} style={{ width: "100%" }} disabled={this.state.ui.disabled}
            onChange={(event) => this.setState({ DesiredTempIdx: parseInt(event.target.value), DesiredTemp: this.idx2Temp(parseInt(event.target.value)) })}
          />
        </Ons.ListItem>
        <Ons.ListItem>
          <div className='right'><Ons.Button modifier="large--cta" onClick={this.handleApplyChanges.bind(this)} disabled={this.state.ui.disabled}>Apply</Ons.Button></div>
        </Ons.ListItem>
      </Ons.List>
    );
  }
}

export default ChangeTemp;
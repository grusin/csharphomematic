import React from 'react';
import * as Ons from 'react-onsenui';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';

import ChangeTemp from './changetemp.js'
import App from '../index.js'

import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import BaseComponent from '../BaseComponent.js';

class Heating extends BaseComponent {
  constructor(props) {
    super(props);

    this.state.ui.toolbar.name = "Heating";
    this.state.ui.toolbar.showBackButton = false;
    this.state.rooms = [];
  }

  componentNeedsData(done) {
    App.Api_GetRooms().then(data => {
      this.setState({ rooms: data });
      if (done != null)
        done();
    }
    );
  }

  renderPage() {
    return (
      <div>
      <Ons.List       
        dataSource={this.state.rooms}
        renderRow={this.renderRow.bind(this)}       
      />     
      </div>
    );
  }  

  getValveOffsetString(d) {
    if(d.Valve_Offset.Value === 0)
      return (d.Level.Value*100).toFixed(0) + '%'
    else 
      return (d.Level.Value*100).toFixed(0) + '% (+' + -d.Valve_Offset.Value.toFixed(0) + '%)'
  }

  renderRow(row) {
    return (
      <Ons.List key={row.ISEID}>
        <Ons.ListHeader>{row.Name}</Ons.ListHeader>
        <Ons.ListItem expandable>
          <div className="left">
            <Ons.Button modifier="large--cta" disabled onClick={this.gotoComponent.bind(this, ChangeTemp, row)} style={{ width: "70px" }}>
              {row.ActualTempAvg.toFixed(1)}&#8451;
                  &nbsp;<Ons.Icon icon="fa-thermometer-empty" />
            </Ons.Button>

            <Ons.Button modifier="large--cta" disabled style={{ width: "70px" }}>
              {row.SetTemp.toFixed(1)}&#8451;
                  &nbsp;<Ons.Icon icon="fa-star" />
            </Ons.Button>

            {row.HumidityAvg > 0 &&
              <Ons.Button modifier="large--cta" disabled style={{ width: "70px" }}>{row.HumidityAvg}%&nbsp;<Ons.Icon icon="fa-tint" /></Ons.Button>
            }

            {row.HeatingActive === true && row.ValveOpenMax > 0 &&
              <Ons.Button modifier="large--cta" disabled style={{ width: "30px" }}><Ons.Icon icon="fa-fire" /> </Ons.Button>
            }

            {row.BoostActive === true &&
              <Ons.Button modifier="large--cta" disabled style={{ width: "30px" }}><Ons.Icon icon="fa-rocket" /> </Ons.Button>
            }

            {row.DehumidifierActive === true &&
              <Ons.Button modifier="large--cta" disabled style={{ width: "30px" }}><Ons.Icon icon="fa-cloud" /> </Ons.Button>
            }

            {row.WindowOpen === true &&
               <Ons.Button modifier="large--cta" disabled style={{ width: "30px" }}><Ons.Icon icon="fa-door-open" /> </Ons.Button>
            }

            {row.Warnings.length > 0 &&
              <Ons.Button modifier="large--cta" style={{ width: "30px" }}><Ons.Icon icon="fa-warning" /></Ons.Button>
            } 
          </div>
          <div className="right">
            <Ons.Button modifier="large--cta" onClick={this.gotoComponent.bind(this, ChangeTemp, { ISEID: row.ISEID })} style={{ width: "30px" }}><Ons.Icon icon="fa-wrench" /></Ons.Button>
          </div>
          <div className="expandable-content">
            <Ons.List 
              dataSource={row.Devices}
              renderHeader={() => 'Devices'}
              renderRow={(d, idx) => (
                <Ons.ListItem modifier='longdivider'>{d.ShortName + ' (' + d.ISEID + ')'} 
                {
                  d.hasOwnProperty('Press_Long') &&  
                  <Ons.Button modifier="material--flat" disabled style={{ padding: "0px 00px 0px 20px", color: "#008CBA" }}>
                    <Ons.Icon icon={d.State ? "fa-toggle-on" : "fa-toggle-off"}/> <Ons.Icon icon="fa-cloud" />
                  </Ons.Button>
                } 
                {
                  d.hasOwnProperty('Actual_Temperature') &&  
                  <Ons.Button modifier="material--flat" disabled style={{ padding: "0px 0px 0px 20px", color: "#008CBA" }}>
                     {d.Actual_Temperature.Value}&#8451;&nbsp;<Ons.Icon icon="fa-thermometer-empty" />
                  </Ons.Button>
                } 
                {
                  d.hasOwnProperty('Valve_State') &&                                  
                                    
                  <Ons.Button modifier="material--flat" disabled style={{ padding: "0px 0px 0px 20px", color: "#008CBA" }}> 
                     {this.getValveOffsetString(d)}&nbsp;
                     <Ons.Icon icon="fa-wrench" />
                  </Ons.Button>
                } 
                {
                  d.hasOwnProperty('Humidity') &&  
                  <Ons.Button modifier="material--flat" disabled style={{ padding: "0px 0px 0px 20px", color: "#008CBA" }}>
                     {d.Humidity.Value}%&nbsp;<Ons.Icon icon="fa-tint" />
                  </Ons.Button>
                } 
                </Ons.ListItem>
              )}
            />        

            {row.Warnings.map((item, i) => <li key={item}>Warning: {item}</li>)}            
          </div>
        </Ons.ListItem>
      </Ons.List>
    );
  }
}

export default Heating;



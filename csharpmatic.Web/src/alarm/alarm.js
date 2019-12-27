import React from 'react';
import * as Ons from 'react-onsenui';
import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';

import App from '../index.js'

import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';
import BaseComponent from '../BaseComponent.js';

import TimeAgo from 'react-timeago'

class Alarm extends BaseComponent {
    constructor(props) {
        super(props);

        this.state.ui.toolbar.name = "G-Alarm";
        this.state.ui.toolbar.showBackButton = false;
        this.state.alarm = [];
        this.state.fabDisabled = true;
        this.state.buttonColorTheme = "gray";
        this.state.AlarmState = "unknown";
    }

    componentNeedsData(done) {
        this.setState({ fabDisabled: true });

        App.Api_GetAlarm().then(a => {
            this.handleNewState(a);
            if (done != null)
                done();
        }
        );
    }

    handleNewState(newState) {
        var niceStatus = "Unknown";
        var themeColor = "gray";
        var onClickAction = null;
        var onClickActionIcon = null;
        
        if (this.state.alarm.AlarmTriggeredEvents == null) {
            //get default values
        }
        if (newState.AlarmArmed === true && newState.AlarmTriggered === true) {
            niceStatus = "Alarm Triggered";
            themeColor = "red";
            onClickAction = this.actionAlarmDisarm.bind(this);
            onClickActionIcon = "fa-unlock";
        }
        else if (newState.AlarmArmed === true && newState.AlarmTriggered === false) {
            niceStatus = "ARMED";
            themeColor = "green";
            onClickAction = this.actionAlarmDisarm.bind(this);
            onClickActionIcon = "fa-unlock";
        }
        else if (newState.AlarmArmed === false && newState.AlarmTriggeredEvents.length === 0) {
            niceStatus = "Disarmed";
            themeColor = "blue";
            onClickAction = this.actionAlarmArm.bind(this);
            onClickActionIcon = "fa-lock";
        }
        else if (newState.AlarmArmed === false && newState.AlarmTriggeredEvents.length !== 0) {
            niceStatus = "Disarmed - Cannot Arm";
            themeColor = "gray"
            onClickAction = null;
            onClickActionIcon = null;
        }

        this.setState({
            alarm: newState
            , fabDisabled: false
            , niceStatus: niceStatus
            , themeColor: themeColor
            , onClickAction: onClickAction
            , onClickActionIcon: onClickActionIcon
        });
    }

    renderPage() {
        if (this.state.alarm.AlarmTriggeredEvents == null)
            return

        return (
            <div>
            <Ons.Button style={{ background: this.state.themeColor }} modifier="large">{this.state.niceStatus}</Ons.Button>,
            
            <Ons.SpeedDial position='right bottom'>
                <Ons.Fab disabled={this.state.onClickAction == null}>
                    <Ons.Icon icon='fa-plus' />
                </Ons.Fab>
                <Ons.SpeedDialItem onClick={this.state.onClickAction} disabled={ (this.state.fabDisabled || this.state.onClickAction == null) ? true : null}><Ons.Icon icon={this.state.onClickActionIcon}/></Ons.SpeedDialItem>
            </Ons.SpeedDial>


            {this.state.alarm.AlarmTriggeredEvents.length !== 0 &&
                    <Ons.List
                        dataSource={this.state.alarm.AlarmTriggeredEvents}
                        renderRow={this.renderAlarmTriggeredEvent.bind(this)}
                        ListHeader={() => <Ons.ListHeader>Triggered Sensors</Ons.ListHeader>}
                    />
            }
            </div>
        );
    }

    renderAlarmTriggeredEvent(e) {
        return <Ons.ListItem key={e.DeviceISEID}>{e.DeviceName}.{e.DatapointName} was triggered&nbsp;<TimeAgo date={e.DatapointTimestamp + 'Z'} /></Ons.ListItem>
    }

    actionAlarmDisarm(event) {
        this.setState({ fabDisabled: true });

        App.Api_SetAlarmDisarmed("1234").then(a => {
            this.handleNewState(a);
        });
    }

    actionAlarmArm(event) {
        this.setState({ fabDisabled: true });

        App.Api_SetAlarmArmed().then(a => {
            this.handleNewState(a);
        });
    }
}

export default Alarm;



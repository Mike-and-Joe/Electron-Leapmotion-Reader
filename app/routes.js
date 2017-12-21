/* eslint flowtype-errors/show-errors: 0 */
import React from 'react'
import { Switch, Route } from 'react-router'
import App from './containers/App'
import HomePage from './containers/HomePage'
import CounterPage from './containers/CounterPage'
import CameraPage from './containers/CameraPage'
import LeapPage from './containers/LeapPage'
// import LeapCameraPage from './containers/LeapCameraPage'
// import LeapJsonPage from './containers/LeapJsonPage'

export default () => (
  <App>
    <Switch>
      <Route path="/counter" component={CounterPage} />
      <Route path="/camera" component={CameraPage} />
      <Route path="/leap" component={LeapPage} />
      {/* <Route path="/leap-camera" component={LeapCameraPage} />
      <Route path="/leap-json" component={LeapJsonPage} /> */}
      <Route path="/" component={HomePage} />
    </Switch>
  </App>
)

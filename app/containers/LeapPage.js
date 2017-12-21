import React from 'react'
import PropTypes from 'prop-types'
import Leap from 'leapjs'
import { Link } from 'react-router-dom'

import { connect } from 'react-redux'

const mapStateToProps = (state, ownProps) => {
  return {
    ...ownProps
  }
}

const mapDispatchToProps = {

}

@connect(mapStateToProps, mapDispatchToProps)
export default class CameraPage extends React.Component {
  state = {
    controller: null
  }

  componentWillMount () {
    const controller = new Leap.Controller()
    this.setState({ controller })
    Leap.Controller.plugin('recordingPlugin', () => {
      return {
        frame: (frame) => {
          console.log('test', frame.hands.length)
        }
      }
    })
    controller.use('recordingPlugin')
    controller.connect()
  }

  componentWillUnmount () {
    this.state.controller.stopUsing('recordingPlugin')
  }

  render () {
    return (
      <div>
        <Link to="/">
          <i className="fa fa-arrow-left fa-3x" />
        </Link>
      </div>
    )
  }
}

CameraPage.propTypes = {

}

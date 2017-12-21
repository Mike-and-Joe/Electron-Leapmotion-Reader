import React from 'react'
import PropTypes from 'prop-types'
import Webcam from 'react-webcam'
import { Link } from 'react-router-dom'

import { connect } from 'react-redux'

const mapStateToProps = (state, ownProps) => {
  return {
    ...ownProps
  }
}

@connect(mapStateToProps)
export default class CameraPage extends React.Component {
  render () {
    return (
      <div>
        <Link to="/">
          <i className="fa fa-arrow-left fa-3x" />
        </Link>
        <Webcam
          audio={false}
          width={350}
          height={350}
          ref={webcam => (this.webcam = webcam)}
          screenshotFormat="image/jpeg"
        />
      </div>
    )
  }
}

CameraPage.propTypes = {

}

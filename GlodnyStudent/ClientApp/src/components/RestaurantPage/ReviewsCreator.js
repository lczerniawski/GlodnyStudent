import React, { Component } from 'react'
import './ReviewsCreator.css';


export default class ReviewsCreator extends Component {

  constructor(props){
    super(props)
    this.handleReviewChange = this.handleReviewChange.bind(this);  
  }

  handleReviewChange(e){
    this.props.onReviewInput(e);
  }


  render() {
    return (
      <form className="reviewForm wow fadeInDown" data-wow-duration="1s" onSubmit={this.props.onSendReview}>
          <textarea name="newReview" type="text" onChange={this.handleReviewChange}></textarea>

          <input  type="submit" value="Wyślij"/>
      </form>
    )
  }
}

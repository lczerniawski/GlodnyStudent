import React, { Component } from 'react'


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
      <form onSubmit={this.props.onSendReview}>
          <textarea name="newReview" type="text" onChange={this.handleReviewChange}></textarea>
          <input  type="submit" value="Wyślij"/>
      </form>
    )
  }
}

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
      <form  onSubmit={this.props.onSendReview}>
          <input name="newReview" type="text" onChange={this.handleReviewChange}></input>
          <input  type="submit" value="Wyślij"/>
      </form>
    )
  }
}

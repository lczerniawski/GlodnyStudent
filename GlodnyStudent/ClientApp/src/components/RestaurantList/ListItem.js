import React, { Component } from 'react'
import PropTypes from 'prop-types';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class listItem extends Component {


  constructor(props){
    super(props);
    this.handleClickElement= this.handleClickElement.bind(this);
  }


  PopularityBadge() {
   if(this.props.reviewsCount > 100) return <div className="PopularityBadge"><i class="fas fa-fire-alt fa-2x"></i><p>Popularne miejsce</p></div>;
  }


  handleClickElement(event) {
     this.context.router.history.push(`/Restauracja/${this.props.id}`);
     event.preventDefault();
   }


  static contextTypes = {
    router: PropTypes.object
  }


  render() {
    const PopularityBadge = this.PopularityBadge();
    return (
        <div onClick={this.handleClickElement} className="RestaurationListItemContainer">
            <img className="restaurantImg" src={imageRestaurant} alt="Zdjecie restauracji" />
          <div className="RestaurationListItemInfo">
            <h3>{this.props.name}</h3>         
            <address>
              <i className="fas fa-map-marker-alt fa-2x" ></i>
               <p>{this.props.address}</p>
            </address>
            <i className="far fa-comments fa-2x" ></i>
            <p>Ilość komentarzy: {this.props.reviewsCount}</p>
              {PopularityBadge} 
            </div>
          </div>
    )
  }
}

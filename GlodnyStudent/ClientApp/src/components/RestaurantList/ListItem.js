import React, { Component } from 'react'
import './ListItem.css';
import PropTypes from 'prop-types';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class listItem extends Component {


  constructor(props){
    super(props);
    this.handleClickElement= this.handleClickElement.bind(this);
  }


  PopularityBadge() {
   if(this.props.reviewsCount > 100) return <p className="badge">Popularne Miejsce</p>;
  }


  handleClickElement(event) {
     this.context.router.history.push(`/Restauracja/${this.props.id}`);
     event.preventDefault();
   }


  static contextTypes = {
    router: PropTypes.object
  }


  render() {
    const badge = this.PopularityBadge();
    return (
        <div className="listItemContainer wow fadeInDown" data-wow-duration="1s" onClick={this.handleClickElement}>
          <div className="image">
            <img src={imageRestaurant} />
          </div>
          <div className="restaurantName">
              <h3>{this.props.name}</h3>
              {badge}
          </div>
          <div className="restaurantInfo">
              <address>
                <i className="fas fa-map-marker-alt fa-2x"></i> {this.props.address}
              </address>

              <ul>
                <li><i className="far fa-comments fa-2x"></i> Ilość komentarzy: {this.props.reviewsCount}</li>
              </ul>
            </div>
          </div>
    )
  }
}

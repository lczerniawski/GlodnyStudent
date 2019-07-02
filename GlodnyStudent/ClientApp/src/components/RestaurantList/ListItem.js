import React, { Component } from 'react'
import PropTypes from 'prop-types';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class listItem extends Component {


  constructor(props){
    super(props);
    this.handleClickElement= this.handleClickElement.bind(this);
  }


  PopularityBadge() {
   if(this.props.reviewsCount > 100) return <p>Popularne Miejsce</p>;
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
        <div onClick={this.handleClickElement}>
          <div>
            <img src={imageRestaurant} />
          </div>
          <div>
              <h3>{this.props.name}</h3>
              {badge}
          </div>
          <div>
              <address>
                <i ></i> {this.props.address} {/* className="fas fa-map-marker-alt fa-2x" */}
              </address>

              <ul>
                <li><i ></i> Ilość komentarzy: {this.props.reviewsCount}</li> {/* className="far fa-comments fa-2x" */}
              </ul>
            </div>
          </div>
    )
  }
}

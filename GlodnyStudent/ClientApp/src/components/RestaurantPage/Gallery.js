import React, { Component } from 'react'

export default class Gallery extends Component {



    render() {

        const galleryList = (this.props.gallery.length !== 0)? this.props.gallery.map((img,i)=><li key= {i} className="wow fadeIn" data-wow-duration="2s"><a data-fancybox="gallery" href={img.dataUrl}>
        <img src={img.dataUrl} /></a><button  name="gallery" value={img.id}  onClick={(e)=>this.props.removeImage(e)}>-</button></li>):<p>Brak zdjęć w galerii.</p>;

        return (
            <div className="gallery">
                  <h3>Galeria</h3>
                  <ul>
                    {galleryList}
                  </ul>
                  <label>Wgraj nowe zdjęcie do galerii: <input type="file" name="gallery" onChange={(e)=>this.props.addImage(e)}/></label>
              </div>
        )
    }
}

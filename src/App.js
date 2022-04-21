import React, { useState } from "react";
import Constants from "./utilities/Constants";
import PostCreateForm from "./components/PostCreateForm";
import PostUpdateForm from "./components/PostUpdateForm";

export default function App() {
  const [posts, setPosts] = useState([]);
  const [showingCreateNewPostForm, setShowingCreateNewPostForm] = useState(false);
  const [postCurrentlyBeingUpdated, setPostCurrentlyBeingUpdated] = useState(null);

  function getPosts() {
    const url = Constants.API_URL_GET_ALL_POSTS;

    fetch(url, { method: 'GET' })
      .then(response => response.json())
      .then(postsFromServer => {
        //console.log(postsFromServer);
        setPosts(postsFromServer);
      })
      .catch(
        (error) => {
          console.log(error);
          alert(error);
        })
  }

  function deletePost(postId){
    const url = `${Constants.API_URL_DELETE_POST_BY_ID}/${postId}`;

    fetch(url, { method: 'DELETE' })
      .then(response => response.json())
      .then(responseFromServer => {
        console.log(responseFromServer);
        onPostDeleted(postId);
      })
      .catch(
        (error) => {
          console.log(error);
          alert(error);
        })
  }

  return (
    <div className="container">
      <div className="row min-vh-100">
        <div className="col d-flex flex-column justify-content-center align-items-center">
          {(showingCreateNewPostForm === false && postCurrentlyBeingUpdated === null) && (
            <div>
              <h1>ASP.NET 6 React App</h1>
              <div className="mt-5">
                <button onClick={getPosts} className="btn btn-primary btn-lg w-100">Get Posts from server</button>
                <button onClick={() => setShowingCreateNewPostForm(true)} className="btn btn-success btn-lg w-100 mt-4">Create New Post</button>
              </div>
            </div>
          )}

          {(posts.length > 0 && showingCreateNewPostForm === false && postCurrentlyBeingUpdated === null) && renderPostsTable()}

          {showingCreateNewPostForm && <PostCreateForm onPostCreated={onPostCreated} />}

          {postCurrentlyBeingUpdated !== null && <PostUpdateForm post={postCurrentlyBeingUpdated} onPostUpdated={onPostDeleted}/>}
        </div>
      </div>
    </div>
  );


  function renderPostsTable() {
    return (
      <div className="table-responsive mt-5">
        <table className="table table-bordered border-dark">
          <thead>
            <tr>
              <th scope="col"> PostId (PK) </th>
              <th scope="col"> Title </th>
              <th scope="col"> Content </th>
              <th scope="col"> CRUD Operations </th>
            </tr>
          </thead>
          <tbody>
            {posts.map((post) => (
              <tr key={post.postId}>
                <th scope="row">{post.postId}</th>
                <td>{post.title}</td>
                <td>{post.content}</td>
                <td>
                  <button onClick={() => setPostCurrentlyBeingUpdated(post)} className="btn btn-primary btn-lg mx-3 my-3">Update</button>
                  <button onClick={() =>{ if(window.confirm(`Are you sure you want to delete the post with title "${post.title}"?`)) deletePost(post.postId) }}className="btn btn-danger btn-lg">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        <button onClick={() => setPosts([])} className="btn btn-dark btn-lg w-100">Clear table</button>

      </div>
    )
  }


  function onPostCreated(createdPost) {
    setShowingCreateNewPostForm(false);

    if (createdPost === null) {
      return;
    }

    alert(`Post successfully created. After clicking OK, your new post titled "${createdPost.title}" will show up in the table below.`)
  }

  function onPostDeleted(updatedPost) {
    setPostCurrentlyBeingUpdated(null);

    if (updatedPost === null) {
      return;
    }

    let postsCopy = [...posts];

    const index = postsCopy.findIndex((postsCopyPost, currentIndex) => {
      if (postsCopyPost.postId === updatedPost.postId) {
        return true;
      }
    });

    if(index !== -1){
      postsCopy[index] = updatedPost;
    }

    setPosts(postsCopy);

    alert(`Post successfully updated.After clicking OK, look for the post with title ${updatedPost.title} in the table below to see the updates.`);
  }

  function onPostDeleted(deletedPostId) {
    let postsCopy = [...posts];

    const index = postsCopy.findIndex((postsCopyPost, currentIndex) => {
      if (postsCopyPost.postId === deletedPostId.postId) {
        return true;
      }
    });

    if(index !== -1){
      postsCopy.splice(index, 1);
    }

    setPosts(postsCopy);

    alert(`Post successfully deleted. After clicking OK look at the table below to see the post dissapear.`);
  }
}


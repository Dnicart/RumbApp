import React, { useEffect, useState } from "react";
import debug from "sabio-debug";
import externalLinksService from "../../services/externalLinksService";
import LinkItem from "./LinkListData";
import { Button } from "react-bootstrap";
import "./stylelinklist.css";
import { useNavigate } from "react-router-dom";
import Table from "react-bootstrap/Table";

const _logger = debug.extend("LinkList");

function LinkListTable() {
  const [link, setLinks] = useState({
    arrayOfLinks: [],
    mappedLinks: [],
  });

  useEffect(() => {
    externalLinksService
      .GetSelectByCreatedBy()
      .then(onLinkSuccess)
      .catch(onLinkError);
  }, []);

  const onLinkSuccess = (response) => {
    _logger("Array of links", response);
    let linkArray = response.items;

    setLinks((prevState) => {
      let newState = { ...prevState };
      newState.arrayOfLinks = linkArray;
      newState.mappedLinks = linkArray.map((link) => ({
        ...link,
      }));
      return newState;
    });
  };

  const onLinkError = (response) => {
    _logger("There was an error.", response);
  };

  const mapLinks = () => {
    //debugger;
    if (!link.mappedLinks.length) {
      return (
        <tr>
          <td colSpan="3">No links available</td>
        </tr>
      );
    } else {
      return link.mappedLinks.map((link) => (
        <LinkItem key={"listA-" + link.id} link={link} />
      ));
    }
  };
  const links = mapLinks();
  const navigate = useNavigate();

  const goToNewLink = () => {
    navigate(`/links/createnewlink`);
  };

  return (
    <React.Fragment>
      <div>
        <Button type="createNewLink" onClick={goToNewLink}>
          Create new Link
        </Button>
      </div>
      <div className="link-list-table">
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Website</th>
              <th>Url</th>
              <th>Edit/Delete</th>
            </tr>
          </thead>
          <tbody>{links}</tbody>
        </Table>
      </div>
    </React.Fragment>
  );
}

export default LinkListTable;

import fs from "fs";
import readline from "readline";

const f = "example.txt";

const lineReader = readline.createInterface({
  input: fs.createReadStream(f),
});

const filterBlack = (tiles) =>
  Object.keys(tiles).reduce((acc, val) => {
    if (tiles[val].isBlack) {
      acc[val] = tiles[val];
    }
    return acc;
  }, {});
const countBlack = (tiles) => Object.keys(filterBlack(tiles)).length;
const flip = (tiles, days) => {
  let day = 0;
  while (day < days) {
    const adj = adjacent(tiles);

    tiles = Object.keys(adj).reduce((acc, val) => {
      const tile = adj[val];

      if (
        (tile.isBlack ? [1, 2] : [2]).find(
          (amount) => amount == tile.adjacent.size
        )
      ) {
        acc[val] = { ...tile, isBlack: true, adjacent: new Set() };
      }
      return acc;
    }, {});

    day++;
  }

  return tiles;
};
const adjacent = (tiles) => {
  return Object.keys(tiles).reduce((acc, val) => {
    const tile = tiles[val];
    const offset = tile.y % 2;

    for (let y = tile.y - 1; y <= tile.y + 1; y++) {
      [
        offset && y != tile.y ? tile.x : tile.x - 1,
        offset || y == tile.y ? tile.x + 1 : tile.x,
      ].forEach((x) => {
        const tileID = `${x},${y}`;

        if (tiles[tileID] && tileID != val) {
          tile.adjacent.add(tileID);
          return;
        }

        if (!acc[tileID]) {
          acc[tileID] = { x, y, isBlack: false, adjacent: new Set() };
        }
        acc[tileID].adjacent.add(val);
      });
    }

    acc[val] = tile;

    return acc;
  }, {});
};

let tiles = [];
lineReader.on("line", (directions) => {
  let pos = [0, 0];
  let offset = false;
  while (directions.length > 0) {
    offset = Math.abs(pos[1] % 2) == 0;
    switch (directions.substr(0, 2)) {
      case "se":
        !offset && pos[0]++;
        pos[1]++;
        directions = directions.substr(2);
        continue;
      case "sw":
        offset && pos[0]--;
        pos[1]++;
        directions = directions.substr(2);
        continue;
      case "nw":
        offset && pos[0]--;
        pos[1]--;
        directions = directions.substr(2);
        continue;
      case "ne":
        !offset && pos[0]++;
        pos[1]--;
        directions = directions.substr(2);
        continue;
    }
    if (directions.substr(0, 1) == "e") {
      pos[0]++;
      directions = directions.substr(1);
      continue;
    }
    pos[0]--;
    directions = directions.substr(1);
    continue;
  }
  let tileID = pos.join(",");
  tiles[tileID] = {
    x: pos[0],
    y: pos[1],
    isBlack: !(tiles[tileID]?.isBlack || false),
    adjacent: new Set(),
  };
});

lineReader.on("close", () => {
  console.log(`part one: ${countBlack(tiles)}`);
  console.log(`part two: ${countBlack(flip(filterBlack(tiles), 100))}`);
});

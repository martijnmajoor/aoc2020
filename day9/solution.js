import fs from "fs";

const notInPreamble = (preamble, lines) => {
  for (let i = preamble; i < lines.length; i++) {
    if (
      !lines
        .slice(i - preamble, i)
        .reduce((acc, cur) => {
          acc.push(
            ...lines
              .slice(i - preamble, i)
              .map((val) => (val != cur ? val + cur : -1))
          );

          return acc;
        }, [])
        .find((val) => val == lines[i])
    ) {
      return lines[i];
    }
  }
};
const weakness = (target, lines) => {
  for (let i = 0; i < lines.length; i++) {
    let sum = lines[i];
    let j = i;
    while (sum < target) {
      j++;
      sum += lines[j];
    }
    if (sum == target && i != j) {
      const range = lines.slice(i, j + 1);
      return Math.min(...range) + Math.max(...range);
    }
  }
};

const preamble = 5, // change preamble to '25' when using actual puzzle input
  file = "example.txt",
  lines = fs
    .readFileSync(file)
    .toString()
    .split("\n")
    .map((line) => parseInt(line.trim(), 10));

const invalid = notInPreamble(preamble, lines);

console.log(`Part one: ${invalid}`);
console.log(`Part two: ${weakness(invalid, lines)}`);

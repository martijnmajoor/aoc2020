package main

import (
	"fmt"
	"strconv"

	"github.com/martijnmajoor/aoc2018/utils/file"
)

func main() {
	fmt.Println("Part one: ", partOne())
	fmt.Println("Part two: ", partTwo())
}
func partOne() int {
	expenses := file.ReadLines("input.txt")
	for k, v := range expenses {
		left, _ := strconv.Atoi(v)
		for k2, v2 := range expenses {
			right, _ := strconv.Atoi(v2)
			if k != k2 && left+right == 2020 {
				return left * right
			}
		}
	}
	return 0
}
func partTwo() int {
	expenses := file.ReadLines("input.txt")
	for k, v := range expenses {
		first, _ := strconv.Atoi(v)
		for k2, v2 := range expenses {
			second, _ := strconv.Atoi(v2)
			for k3, v3 := range expenses {
				third, _ := strconv.Atoi(v3)
				if k != k2 && k != k3 && first+second+third == 2020 {
					return first * second * third
				}
			}
		}
	}
	return 0
}

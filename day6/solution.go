package main

import (
	"fmt"
	"strings"

	"github.com/martijnmajoor/aoc2018/utils/file"
)

func main() {
	any, all := answers(file.ReadLines("input.txt"))
	fmt.Println("Part one: ", any)
	fmt.Println("Part two: ", all)
}

func answers(lines []string) (any int, all int) {
	yes := make(map[string]int)
	group := 0

	tally := func() {
		for _, amount := range yes {
			any++

			if amount == group {
				all++
			}
		}
	}

	for _, line := range lines {
		if line == "" {
			tally()

			yes = make(map[string]int)
			group = 0

			continue
		}

		for _, answer := range strings.Split(line, "") {
			yes[answer]++
		}

		group++
	}

	tally()

	return
}

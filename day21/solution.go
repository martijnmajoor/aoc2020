package main

import (
	"bufio"
	"fmt"
	"os"
	"sort"
	"strings"
)

// Recipe contains allergen information and ingredients for a recipe
type Recipe struct {
	allergens   []string
	ingredients []string
}

// NewRecipe constructs a new recipe
func NewRecipe(allergens []string, ingredients []string) Recipe {
	return Recipe{
		allergens:   allergens,
		ingredients: ingredients,
	}
}

func main() {
	recipes := parse("example.txt")
	translations := translate(recipes)
	fmt.Println("part one:", len(unknown(translations, recipes)))
	fmt.Println("part two:", canonical(translations))
}
func parse(fn string) (result []Recipe) {
	file, _ := os.Open(fn)
	defer file.Close()

	result = make([]Recipe, 0)
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.Split(strings.Replace(scanner.Text(), ")", "", -1), "(contains ")

		result = append(result, NewRecipe(
			strings.Split(line[1], ", "),
			strings.Split(strings.TrimRight(line[0], " "), " "),
		))
	}

	return
}
func translate(recipes []Recipe) (translations map[string]string) {
	translations = make(map[string]string, 0)
	for len(recipes) > 0 {
		cleanup := make([]string, 0)
		for _, recipe := range recipes {
			if len(recipe.allergens) == 1 {
				allergen := recipe.allergens[0]
				matches := hasAllergen(allergen, recipes)
				valid := make([]string, 0)
				for _, ingredient := range recipe.ingredients {
					if _, translated := translations[ingredient]; !translated && matchesAll(ingredient, matches) {
						valid = append(valid, ingredient)
					}
				}
				if len(valid) == 1 {
					translations[valid[0]] = allergen
					cleanup = append(cleanup, allergen)
				}
			}
		}
		for _, allergen := range cleanup {
			recipes = removeAllergen(allergen, recipes)
		}
	}
	return
}
func unknown(translations map[string]string, recipes []Recipe) (result []string) {
	for _, recipe := range recipes {
		for _, ingredient := range recipe.ingredients {
			if _, ok := translations[ingredient]; !ok {
				result = append(result, ingredient)
			}
		}
	}
	return
}
func canonical(translations map[string]string) (result string) {
	keys := make([]string, 0)
	for translation := range translations {
		keys = append(keys, translation)
	}

	sort.Slice(keys, func(i, j int) bool {
		return translations[keys[i]] < translations[keys[j]]
	})

	var b strings.Builder
	for _, key := range keys {
		fmt.Fprintf(&b, "%s,", key)
	}
	return strings.TrimRight(b.String(), ",")
}
func hasAllergen(allergen string, recipes []Recipe) (result [][]string) {
	for _, recipe := range recipes {
		for _, match := range recipe.allergens {
			if allergen == match {
				result = append(result, recipe.ingredients)
			}
		}
	}
	return
}
func matchesAll(ingredient string, recipes [][]string) bool {
	for _, recipe := range recipes {
		valid := false
		for _, item := range recipe {
			if ingredient == item {
				valid = true
			}
		}
		if !valid {
			return false
		}
	}
	return true
}
func removeAllergen(allergen string, recipes []Recipe) (result []Recipe) {
	for _, recipe := range recipes {
		allergens := make([]string, 0)
		for _, match := range recipe.allergens {
			if allergen != match {
				allergens = append(allergens, match)
			}
		}
		if len(allergens) > 0 {
			result = append(result, NewRecipe(allergens, recipe.ingredients))
		}
	}
	return
}

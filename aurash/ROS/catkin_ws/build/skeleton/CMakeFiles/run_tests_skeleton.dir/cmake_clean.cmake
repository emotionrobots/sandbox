FILE(REMOVE_RECURSE
  "CMakeFiles/run_tests_skeleton"
)

# Per-language clean rules from dependency scanning.
FOREACH(lang)
  INCLUDE(CMakeFiles/run_tests_skeleton.dir/cmake_clean_${lang}.cmake OPTIONAL)
ENDFOREACH(lang)

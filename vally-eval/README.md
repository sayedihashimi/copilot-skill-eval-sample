# .NET Performance Analysis — Vally Evaluation

This is the Vally equivalent of the `evals/dotnet-perf` copilot-skill-eval evaluation.
It evaluates how [dotnet/skills](https://github.com/dotnet/skills) performance skills
improve Copilot's ability to detect .NET performance anti-patterns.

## Prerequisites

- Node.js 20+
- `@microsoft/vally-cli` (`npx @microsoft/vally-cli` or install globally)
- A clone of the [dotnet/skills](https://github.com/dotnet/skills) repo

## Setup

1. **Clone dotnet/skills** (if you haven't already):

   ```bash
   git clone https://github.com/dotnet/skills ../dotnet-skills-repo
   ```

2. **Update skill paths** in `.vally.yaml` to point to your clone:

   ```yaml
   environments:
     dotnet-perf-skills:
       skills:
         - ../dotnet-skills-repo/plugins/dotnet-diag/skills/analyzing-dotnet-performance
         - ../dotnet-skills-repo/plugins/dotnet-diag/skills/microbenchmarking
         - ../dotnet-skills-repo/plugins/dotnet-diag/skills/dotnet-trace-collect
   ```

3. **Verify the perf01 sample** exists at `../samples/perf01/` (relative to this directory).

## Usage

### Run the full evaluation (both baseline and with-skills)

```bash
cd vally-eval
npx @microsoft/vally-cli eval --eval-spec evals/eval.yaml --output-dir results --verbose
```

### Run with more trials for reliability

```bash
npx @microsoft/vally-cli eval --eval-spec evals/eval.yaml --output-dir results --runs 5
```

### Run only baseline or only with-skills

```bash
# Baseline only
npx @microsoft/vally-cli eval --eval-spec evals/eval.yaml --output-dir results --tag configuration=baseline

# With skills only
npx @microsoft/vally-cli eval --eval-spec evals/eval.yaml --output-dir results --tag configuration=dotnet-perf-skills
```

### Compare two runs (pairwise)

```bash
npx @microsoft/vally-cli compare \
  --eval-spec evals/eval.yaml \
  --run-a results/<baseline-run>/ \
  --run-b results/<skills-run>/
```

### Lint the skills (static checks)

```bash
npx @microsoft/vally-cli lint ../dotnet-skills-repo/plugins/dotnet-diag/skills
```

## Configuration Mapping

| copilot-skill-eval concept | Vally equivalent |
|---|---|
| `eval.yaml` scenarios | `stimuli` in eval.yaml |
| `configurations` (skill sets) | `environments` in .vally.yaml |
| `dimensions` (quality criteria) | `prompt` graders with rubrics |
| `tier` (critical/high/moderate) | `scoring.weights` (3.0/2.0/1.0) |
| `include_directories` | `environment.files` (src/dest pairs) |
| `skill-sources.yaml` | `environment.skills` paths in .vally.yaml |
| `skill-eval run` | `vally eval --eval-spec ...` |
| `skill-eval analyze` | Built-in — grading runs automatically |

## Output

Results are written to the `results/` directory (timestamped subfolders):
- `results.jsonl` — Raw results in JSONL format
- `eval-results.md` — Human-readable markdown report with per-grader scores
- `eval-results.junit.xml` — JUnit XML (if `--junit` is passed)

# Copilot Instructions

## Project Guidelines
- Prefer 'get all/list' queries to return success with an empty list rather than failing or throwing when no data exists; use NotFoundException primarily for single-resource lookups or update/delete when the id is missing.
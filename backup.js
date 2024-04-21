#!/usr/bin/env zx

import { $ } from 'zx'
import os from 'node:os'
import path from 'node:path'
import makeDir from 'make-dir'
import { rimraf } from 'rimraf'
import fs from 'fs/promises'

// $.verbose = false;
(async () => {
  const config = (await $`cat config`)
  let  configArr = config.stdout.split('\n')
  configArr = configArr.filter(item => item && !item.startsWith('#')).map(item => {
    let arr = item.split(' ')
    let url = arr[0]
    let branch = arr[1] || 'main'
    let dir = arr[2] || (new URL(arr[0]).pathname.slice(1))
    return {
      url,
      branch,
      dir
    }
  })
  console.log(configArr)
  let tmp = os.tmpdir()

  tmp = path.resolve(tmp, 'backup-repo/repo')
  console.log(tmp)
  await rimraf(tmp)
  await makeDir(tmp)

  // await $`apt install git-filter-repo -y`
  for(const item of configArr) {
    // $.cwd = process.cwd()
    let name = item.dir.replace('/', '@')
    // await $`npx rimraf repo/${name}`
    // await $`git clone ${item.url} repo/${name}`
    // $.cwd = `repo/${name}`
    // await $`git remote remove backup &>/dev/null || true`
    // await $`git remote add backup git@github.com:wqdygkd/backup-repo.git &>/dev/null || true`
    // await $`git push -u backup ${item.branch}:${name} &>/dev/null || true`
    // await $`npx rimraf .git`
    // await $`git add .`
    // let date = $`date`
    // await $`git commit -m ${date}:backup${name}`

    await $`git remote remove ${name} &>/dev/null || true `
    await $`git remote add ${name} ${item.url}`
    await $`git fetch ${name} -p`
    await $`git branch -D ${name} &>/dev/null || true`
    await $`git checkout -b ${name} ${name}/${item.branch}`

    let cp = 'cp -r ' + path.resolve('./') + ' ' +  path.resolve(tmp, name)
    console.log(cp)
    await $(cp)
    // await copyDir()
    // await copyDir('./', tmp)



    // await $`git filter-repo --to-subdirectory-filter ${item.dir}  -f`
    // await $`git checkout main`
    // await $`git merge ${name} --allow-unrelated-histories`
  }
})()





async function copyDir(src, dest) {
  const entries = await fs.readdir(src, { withFileTypes: true });

  await fs.mkdir(dest, { recursive: true });

  for (let entry of entries) {
    const srcPath = path.join(src, entry.name);
    const destPath = path.join(dest, entry.name);

    if (entry.isDirectory()) {
      await copyDir(srcPath, destPath);
    } else {
      await fs.copyFile(srcPath, destPath);
    }
  }
}
